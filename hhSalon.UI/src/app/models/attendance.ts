import { Time } from "@angular/common"

export class Attendance{
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