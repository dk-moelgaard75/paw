export interface IEmployee {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    password: string;
    employeeType: string;
    token?: string;
    uid: string;
}