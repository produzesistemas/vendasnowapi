import { CreditCard } from './credit-card-model';
export class Payment {
    Type: string;
    Amount: number;
    Installments: number;
    Provider: string;
    SoftDescriptor: string;
    CreditCard: CreditCard;
    Capture: boolean;
    Authenticate: boolean;
}
