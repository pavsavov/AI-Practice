\## Plan: Simple MAUI Image Library Desktop App



A performant .NET MAUI desktop app for batch-importing images, extracting their metadata, and organizing them into a date-based library. The plan strictly follows the requirements in CONTEXT.md and CODE\_QALITY\_RULES.md, ensuring modularity, testability, and clean architecture.



\*\*Steps\*\*



1\. \*\*Requirements \& Idea Documentation\*\*

&nbsp;  - Summarize the app’s purpose: batch image import, metadata extraction, date-based sorting, and configuration-driven folder naming.

&nbsp;  - Document limitations (e.g., EXIF stripping by chat apps) and supported formats (.jpeg, .jpg, .bmp, etc.).



2\. \*\*System Architecture \& Modules\*\*

&nbsp;  - \*\*UI Module (Zalmo.UI)\*\*

&nbsp;    - Screen 1: Welcome, app description, configuration fields (batch size, folder naming pattern), and disclaimer.

&nbsp;    - Screen 2: Dropdown for selecting a single image or folder; triggers import and grouping.

&nbsp;    - MVVM pattern, DI for services.

&nbsp;  - \*\*Backend Module (Zalmo.Core, Zalmo.Infrastructure)\*\*

&nbsp;    - Metadata extraction service using MetadataExtractor.

&nbsp;    - Fallback to file creation time if EXIF date missing.

&nbsp;    - Parallel grouping and file moving logic.

&nbsp;    - Configuration persistence (JSON file).

&nbsp;    - Logging via Microsoft.Extension.Logging.

&nbsp;  - \*\*Testing Module (Zalmo.Tests)\*\*

&nbsp;    - Unit tests for metadata extraction, grouping, and configuration logic.

&nbsp;    - Use NSubstitute for mocking, AutoFixture for test data.



3\. \*\*Development Process per Module\*\*

&nbsp;  - For each module:

&nbsp;    - Define approach and reasoning (e.g., why MVVM, why parallelization).

&nbsp;    - List step-by-step workflow for Copilot/Cursor/Claude Code prompts.

&nbsp;    - Specify testing strategy (unit tests, manual UI checks).

&nbsp;    - Document which AI tool was used and why.

&nbsp;    - Record 2–3 key prompts/interactions.



4\. \*\*Implementation Guidelines\*\*

&nbsp;  - Adhere to coding standards: method/class size, naming, SOLID, error handling, logging.

&nbsp;  - Use DI, keep methods under 50 lines, single responsibility.

&nbsp;  - Place all tests in dedicated test projects.

&nbsp;  - No AutoMapper unless justified by complexity.



5\. \*\*Edge Cases \& Performance\*\*

&nbsp;  - Handle missing EXIF gracefully.

&nbsp;  - Only process valid image files.

&nbsp;  - Ensure parallel grouping is thread-safe and performant.



6\. \*\*Documentation \& Evidence\*\*

&nbsp;  - Update ARCHITECTURE.md with the new module breakdown.

&nbsp;  - Prepare screenshots of both UI screens and a sample DUMP folder.

&nbsp;  - Document challenges, tool comparison, and improvement ideas.



\*\*Verification\*\*

\- Run the app: verify both screens, configuration saving, and correct grouping in DUMP.

\- Check logs for structured entries.

\- Run all unit tests (metadata extraction, grouping, config).

\- Review documentation for completeness and clarity.



\*\*Decisions\*\*

\- Chose .NET MAUI for cross-platform desktop UI.

\- Used MetadataExtractor for robust metadata handling.

\- Used parallel processing for performance.

\- Followed strict separation of concerns and test project organization per CODE\_QALITY\_RULES.md.

