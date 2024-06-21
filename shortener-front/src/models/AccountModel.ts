import ShortenModel from "./ShortenModel.ts"

export default interface AccountModel {
    username: string
    email: string
    createdAt: Date
    shortens: ShortenModel[]
}
