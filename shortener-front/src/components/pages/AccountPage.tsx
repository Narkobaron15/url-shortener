import {useNavigate} from "react-router-dom"
import AccountModel from "../../models/AccountModel.ts"
import http_common from "../../common/http_common.ts"
import {useEffect, useState} from "react"
import {Table} from "flowbite-react"
import './css/AccountPage.css'
import APP_ENV from "../../common/env.ts"

export default function AccountPage() {
    const navigate = useNavigate()
    const [account, setAccount] = useState<AccountModel | null>(null)

    useEffect(() => {
        if (!localStorage.getItem('auth'))
            navigate('/login')

        http_common.get('/user/info')
            .then(response => {
                setAccount(response.data)
            })
            .catch(error => {
                console.error('Error getting account info', error)
                navigate('/login')
            })
    }, [navigate])

    const deleteShorten = (shortenId: string) => {
        http_common.delete(`/user/route/${shortenId}`)
            .then(response => {
                console.log('Shorten deleted', response)
                window.location.reload()
            })
            .catch(error => {
                console.error('Error deleting shorten', error)
            })
    }

    return account
        ? (
            <div className="p-4">
                <h1 className="headline">{account.username}'s account Page</h1>
                <div className="mb-4">
                    <p><strong>User name:</strong> {account.username}</p>
                    <p><strong>Email:</strong> {account.email}</p>
                    <p><strong>Created At:</strong> {new Date(account.createdAt).toDateString()}</p>
                </div>

                <h2 className="text-xl font-bold mb-4">Shortened URLs</h2>
                <Table striped={true}>
                    <Table.Head>
                        <Table.HeadCell>Shortened URL</Table.HeadCell>
                        <Table.HeadCell>Original URL</Table.HeadCell>
                        <Table.HeadCell>Created At</Table.HeadCell>
                        <Table.HeadCell>Clicks</Table.HeadCell>
                        <Table.HeadCell>Actions</Table.HeadCell>
                    </Table.Head>
                    <Table.Body className="divide-y">
                        {account.shortens.map((shorten, index) => (
                            <Table.Row key={index}>
                                <Table.Cell>
                                    <a className="link" href={`${APP_ENV.BASE_URL}/${shorten.code}`}
                                       target="_blank" rel="noopener noreferrer">
                                        {shorten.code}
                                    </a>
                                </Table.Cell>
                                <Table.Cell>
                                    {shorten.url}
                                </Table.Cell>
                                <Table.Cell>{new Date(shorten.createdAt).toDateString()}</Table.Cell>
                                <Table.Cell>{shorten.clicks}</Table.Cell>
                                <Table.Cell>
                                    {/*<button className="btn btn-sm btn-primary">Edit</button>*/}
                                    <button className="delete-btn" onClick={
                                        () => deleteShorten(shorten.code)
                                    }>Delete</button>
                                </Table.Cell>
                            </Table.Row>
                        ))}
                    </Table.Body>
                </Table>
            </div>
        ) :
        (<></>) // TODO: add a loading spinner
}
