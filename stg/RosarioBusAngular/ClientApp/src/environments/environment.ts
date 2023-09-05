// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
    production: false,
    identityUrl: 'http://localhost:64039',
    planificacionUrl: 'http://localhost:64039',
    siniestrosUrl: 'http://localhost:64040',
    reportUrl: 'http://localhost:64041',
    artUrl: 'http://localhost:64042',
    firmaDigitalUrl: 'http://localhost:64044',
    logsUrl: 'http://localhost:53605',
    signalrUrl: 'http://localhost:54616/rbushub',
    version: '1.0.0.0'
};
