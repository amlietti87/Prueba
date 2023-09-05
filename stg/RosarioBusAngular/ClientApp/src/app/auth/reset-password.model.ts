export class ResetPasswordInput implements IResetPasswordInput {
    userId: number;
    resetCode: string;
    password: string;
    returnUrl: string;
    singleSignIn: string;

    constructor(data?: IResetPasswordInput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.userId = data["userId"];
            this.resetCode = data["resetCode"];
            this.password = data["password"];
            this.returnUrl = data["returnUrl"];
            this.singleSignIn = data["singleSignIn"];
        }
    }

    static fromJS(data: any): ResetPasswordInput {
        let result = new ResetPasswordInput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userId"] = this.userId;
        data["resetCode"] = this.resetCode;
        data["password"] = this.password;
        data["returnUrl"] = this.returnUrl;
        data["singleSignIn"] = this.singleSignIn;
        return data;
    }
}

export interface IResetPasswordInput {
    userId: number;
    resetCode: string;
    password: string;
    returnUrl: string;
    singleSignIn: string;
}



export class ResetPasswordModel extends ResetPasswordInput {
    public passwordRepeat: string;
}


export class ResetPasswordOutput implements IResetPasswordOutput {
    canLogin: boolean;
    userName: string;
    errorMesagge: string;

    constructor(data?: IResetPasswordOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.canLogin = data["canLogin"];
            this.userName = data["userName"];
        }
    }

    static fromJS(data: any): ResetPasswordOutput {
        let result = new ResetPasswordOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["canLogin"] = this.canLogin;
        data["userName"] = this.userName;
        return data;
    }
}

export interface IResetPasswordOutput {
    canLogin: boolean;
    userName: string;
}
