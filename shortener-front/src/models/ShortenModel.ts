export default interface ShortenModel {
    code: string
    url: string
    createdAt: Date
    expiresAt: Date
    clicks: number
}
