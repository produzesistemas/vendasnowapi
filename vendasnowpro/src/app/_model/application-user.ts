export class ApplicationUser {
    id: string;
    providerId: string;
    provider: string;
    email: string;
    userName: string;
    phoneNumber: string;
    emailConfirmed: boolean;
    phoneNumberConfirmed: boolean;
    twoFactorEnabled: boolean;
    lockoutEnabled: boolean;
    accessFailedCount: number;

    cpf: string;
    name: string;


    public constructor(init?: Partial<ApplicationUser>) {
        Object.assign(this, init);
    }
}
