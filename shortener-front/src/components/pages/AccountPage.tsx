import {useNavigate} from "react-router-dom"
import AccountModel from "../../models/AccountModel.ts"
import http_common from "../../common/http_common.ts"
import {useEffect, useState} from "react"

export default function AccountPage() {
    const navigate = useNavigate()
    const [account, setAccount] = useState<AccountModel | null>(null)

    if (!localStorage.getItem('auth'))
        navigate('/login')

    useEffect(() => {
        http_common.get('/user/info')
            .then(response => {
                setAccount(response.data)
            })
            .catch(error => {
                console.error('Error getting account info', error)
                navigate('/login')
            })
    }, [])

    return account
        ? (
            <>
                <h1>{account.username}'s account Page</h1>
            </>
        ) : (<></>) // TODO: add a loading spinner
}