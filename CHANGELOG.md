# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Indexes can be separated in the same line by ',' and ';' as well as ' '. Empty entries will be removed.
- Latest saved file is selected upon output folder opening.

### Changed

- Output folder opening with a button instead of automatically after saving file.

## [1.0.1] - 2024 - 03 - 11

### Fixed

- Only latest index was read on file loading.

## [1.0.0] - 2024 - 03 - 09

### Added

- Creation of user selection file for Sysmac Studio Parameter Editor.
- Save file button tooltip to point out required fields.
- Loading of user selection file.
- SharedAssemblyInfo file.
- Application version on main window get from assembly info.
- Building and deployment scripts.
- In session persistence of latest load/save folder.
