using Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods on <see cref="IDbContextFactory{BizyPopDbContext}"/> for
    /// scoped database access without manually managing context lifetimes.
    /// <para>
    /// Two operation categories are available:
    /// <list type="bullet">
    ///   <item><description>
    ///     <b>Write</b> — <c>WriteWithDbContextAsync</c>: wraps the operation in a database
    ///     transaction. Automatically commits on success and rolls back on any exception.
    ///     Use for <c>INSERT</c>, <c>UPDATE</c>, and <c>DELETE</c> operations.
    ///   </description></item>
    ///   <item><description>
    ///     <b>Query</b> — <c>QueryWithDbContextAsync</c>: no transaction overhead.
    ///     Use for read-only <c>SELECT</c> operations.
    ///   </description></item>
    ///   <item><description>
    ///     <b>Paginate</b> — <c>PaginateAsync</c>: executes a count and a paged fetch in one
    ///     call and returns a <see cref="PaginationResponse{T}"/> ready for the API response.
    ///   </description></item>
    /// </list>
    /// </para>
    /// </summary>
    public static class DbContextFactoryExtensions
    {
        extension(IDbContextFactory<BizyPopDbContext> factory)
        {
            /// <summary>
            /// Executes a write operation inside a database transaction and returns a result.
            /// <para>
            /// A fresh <see cref="BizyPopDbContext"/> is created for every call, so this method
            /// is safe to invoke in parallel — each call gets its own isolated context and
            /// transaction.
            /// </para>
            /// </summary>
            /// <typeparam name="T">The type of the value returned by <paramref name="resultAction"/>.</typeparam>
            /// <param name="resultAction">
            /// An async delegate that receives the scoped <see cref="BizyPopDbContext"/> and
            /// performs the write operation. Call <c>SaveChangesAsync</c> inside the delegate
            /// to persist changes.
            /// </param>
            /// <returns>The value produced by <paramref name="resultAction"/>.</returns>
            /// <exception cref="Exception">
            /// Any exception thrown inside <paramref name="resultAction"/> triggers a rollback
            /// before being re-thrown to the caller.
            /// </exception>
            public async Task<T> WriteWithDbContextAsync<T>(
            Func<BizyPopDbContext, Task<T>> resultAction)
            {
                using var context = await factory.CreateDbContextAsync().ConfigureAwait(false);
                using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var result = await resultAction(context).ConfigureAwait(false);
                    await transaction.CommitAsync().ConfigureAwait(false);
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            }

            /// <summary>
            /// Executes a write operation inside a database transaction with no return value.
            /// <para>
            /// A fresh <see cref="BizyPopDbContext"/> is created for every call, so this method
            /// is safe to invoke in parallel — each call gets its own isolated context and
            /// transaction.
            /// </para>
            /// </summary>
            /// <param name="resultAction">
            /// An async delegate that receives the scoped <see cref="BizyPopDbContext"/> and
            /// performs the write operation. Call <c>SaveChangesAsync</c> inside the delegate
            /// to persist changes.
            /// </param>
            /// <exception cref="Exception">
            /// Any exception thrown inside <paramref name="resultAction"/> triggers a rollback
            /// before being re-thrown to the caller.
            /// </exception>
            public async Task WriteWithDbContextAsync(
                Func<BizyPopDbContext, Task> resultAction)
            {
                using var context = await factory.CreateDbContextAsync().ConfigureAwait(false);
                using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

                try
                {
                    await resultAction(context).ConfigureAwait(false);
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            }

            /// <summary>
            /// Executes a read-only query and returns a result. No transaction is started,
            /// making this more efficient than <c>WriteWithDbContextAsync</c> for
            /// <c>SELECT</c>-only operations.
            /// </summary>
            /// <typeparam name="T">The type of the value returned by <paramref name="queryAction"/>.</typeparam>
            /// <param name="queryAction">
            /// An async delegate that receives the scoped <see cref="BizyPopDbContext"/> and
            /// performs the read operation (e.g. <c>AnyAsync</c>, <c>FirstOrDefaultAsync</c>,
            /// <c>ToListAsync</c>).
            /// </param>
            /// <returns>The value produced by <paramref name="queryAction"/>.</returns>
            public async Task<T> QueryWithDbContextAsync<T>(
                Func<BizyPopDbContext, Task<T>> queryAction)
            {
                using var context = await factory.CreateDbContextAsync().ConfigureAwait(false);
                return await queryAction(context).ConfigureAwait(false);
            }

            /// <summary>
            /// Executes a read-only operation with no return value. No transaction is started.
            /// Useful for fire-and-forget style reads or warming up queries.
            /// </summary>
            /// <param name="queryAction">
            /// An async delegate that receives the scoped <see cref="BizyPopDbContext"/> and
            /// performs the read operation.
            /// </param>
            public async Task QueryWithDbContextAsync(
                Func<BizyPopDbContext, Task> queryAction)
            {
                using var context = await factory.CreateDbContextAsync().ConfigureAwait(false);
                await queryAction(context).ConfigureAwait(false);
            }

            /// <summary>
            /// Executes a paged query and returns a fully populated <see cref="PaginationResponse{T}"/>.
            /// Runs a <c>COUNT</c> and a <c>LIMIT/OFFSET</c> fetch in a single context lifetime.
            /// No transaction is started.
            /// </summary>
            /// <typeparam name="T">The element type of the query result.</typeparam>
            /// <param name="queryAction">
            /// A delegate that receives the scoped <see cref="BizyPopDbContext"/> and returns the
            /// base <see cref="IQueryable{T}"/> to paginate. Apply filtering and ordering here;
            /// do <b>not</b> call <c>Skip</c>/<c>Take</c> — pagination is handled internally.
            /// </param>
            /// <param name="request">
            /// Pagination parameters (<c>PageNumber</c>, <c>PageSize</c>) sourced from the
            /// incoming request.
            /// </param>
            /// <returns>
            /// A <see cref="PaginationResponse{T}"/> containing the current page of items along
            /// with total count, total pages, and <c>HasNextPage</c> / <c>HasPreviousPage</c> flags.
            /// </returns>
            public async Task<PaginationResponse<T>> PaginateAsync<T>(
                Func<BizyPopDbContext, IQueryable<T>> queryAction,
                PaginationRequest request)
            {
                using var context = await factory.CreateDbContextAsync().ConfigureAwait(false);

                var query = queryAction(context);
                var totalCount = await query.CountAsync().ConfigureAwait(false);

                var pageNumber = request.ValidPageNumber;
                var pageSize = request.ValidPageSize;
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return new PaginationResponse<T>(
                    Items: items.AsReadOnly(),
                    PageNumber: pageNumber,
                    PageSize: pageSize,
                    TotalCount: totalCount,
                    TotalPages: totalPages,
                    HasNextPage: pageNumber < totalPages,
                    HasPreviousPage: pageNumber > 1);
            }

            /// <summary>
            /// Executes a paged query, optionally transforms each item (e.g. to resolve absolute
            /// image URLs), and returns a fully populated <see cref="PaginationResponse{T}"/>.
            /// Runs a <c>COUNT</c> and a <c>LIMIT/OFFSET</c> fetch in a single context lifetime.
            /// No transaction is started.
            /// </summary>
            /// <typeparam name="T">The element type of the query result.</typeparam>
            /// <param name="queryAction">
            /// A delegate that receives the scoped <see cref="BizyPopDbContext"/> and returns the
            /// base <see cref="IQueryable{T}"/> to paginate. Apply filtering and ordering here;
            /// do <b>not</b> call <c>Skip</c>/<c>Take</c> — pagination is handled internally.
            /// </param>
            /// <param name="request">
            /// Pagination parameters (<c>PageNumber</c>, <c>PageSize</c>) sourced from the
            /// incoming request.
            /// </param>
            /// <param name="urlMapperFunc">
            /// An optional in-memory projection applied to each item after the database fetch.
            /// Typically used to convert relative image paths to absolute URLs using
            /// <c>IHttpContextAccessor</c>. Pass <see langword="null"/> to skip transformation.
            /// </param>
            /// <returns>
            /// A <see cref="PaginationResponse{T}"/> containing the current page of transformed
            /// items along with total count, total pages, and <c>HasNextPage</c> /
            /// <c>HasPreviousPage</c> flags.
            /// </returns>
            public async Task<PaginationResponse<T>> PaginateAsync<T>(
                Func<BizyPopDbContext, IQueryable<T>> queryAction,
                PaginationRequest request,
                Func<T, T>? urlMapperFunc = null)
            {
                using var context = await factory.CreateDbContextAsync().ConfigureAwait(false);

                var query = queryAction(context);
                var totalCount = await query.CountAsync().ConfigureAwait(false);

                var pageNumber = request.ValidPageNumber;
                var pageSize = request.ValidPageSize;
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (urlMapperFunc is not null)
                {
                    items = items.Select(urlMapperFunc).ToList();
                }

                return new PaginationResponse<T>(
                    Items: items.AsReadOnly(),
                    PageNumber: pageNumber,
                    PageSize: pageSize,
                    TotalCount: totalCount,
                    TotalPages: totalPages,
                    HasNextPage: pageNumber < totalPages,
                    HasPreviousPage: pageNumber > 1);
            }
        }
    }
}
