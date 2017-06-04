Platinum
=========================================================================

Platinum is an opinionated base framework, aimed towards productivity
rather than flexibility. Extensibility is sometimes supported, where
strictly necessary -- but even then, in a directed manner.

| Assembly   | Description
|------------|-----------------------------------------------------------
| Core       | Core functionality: exception handling, configuration management, and lots of utilities.
| Data       | Simple data-access, wrapping around [Dapper](https://github.com/StackExchange/Dapper).
| Database   | Auto-run SQL scripts, extending [DbUp](https://dbup.github.io/)
| Logging    | Emit event / logging data into Elastic Search, wrapping around [NLog](https://github.com/NLog/NLog/).
| Metrics    | Emit metric data into Elastic Search, through `NLog`.
| Mock       | Random and dictionary based random-data generation for classes.
| Validation | Attribute based property validation.
