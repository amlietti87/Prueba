export const PFXERROR = 'error';
export const SESSION_EXPIRED = 'session-expired';
export const CONNECTION_REFUSED = 'connection-refused';
export const INCORRECT_USER_PASS = 'incorrect-user-pass';

export class Error {
    key: string;
    value: string;
};

export const ERRORS: Error[] = [
    { key: SESSION_EXPIRED, value: 'LOGIN.EXPIRED' },
    { key: CONNECTION_REFUSED, value: 'APPLICATION.CONNECTIONREFUSED' },
    { key: INCORRECT_USER_PASS, value: 'LOGIN.ERROR' }
];
