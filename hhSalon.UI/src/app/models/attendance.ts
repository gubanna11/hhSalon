import { Time } from "@angular/common"

export class Attendance{
    id?:number;
    clientId?: string
    groupId?: number
    serviceId?: number
    workerId?: string
    date?:Date
    time?:string
    price?: number
    isRendered?:string
    isPaid?:string
}