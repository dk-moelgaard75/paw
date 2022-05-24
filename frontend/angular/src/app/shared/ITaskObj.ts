export interface ITaskObj {
    id: number;
    taskName: string;
    description: string;
    startDate: string;
    startHour: number;
    estimatedHours: number;
    endDate: string;
    taskGuid: string;
    customerGuid: string;
    employee: string;
}
