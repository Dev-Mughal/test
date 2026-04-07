# Copilot Instructions

## General Guidelines
- Always use HasColumnType("text") for all string properties in EF Core entity configurations, regardless of whether they are required or optional.
- Prefer direct file edits over using git commands in the terminal during implementation.

## Image Handling
- Offload image handling to ImageStorageAPI; the BizyPop API should store only returned image paths and prepend the ImageServer BaseUrl in responses.