import {Button, Card, TextInput} from "flowbite-react"
import {useState} from "react"
import './css/HomePage.css'
import {useNavigate} from "react-router-dom"
import http_common from "../../common/http_common.ts"
import {HiClipboardCopy} from "react-icons/hi"

export default function HomePage() {
    const [url, setUrl] = useState('')
    const [shortenedUrl, setShortenedUrl] = useState('')
    const [error, setError] = useState<string | null>(null)
    const [recentCopied, setRecentCopied] = useState<boolean>(false)

    const navigate = useNavigate()

    const handleShortenUrl = () => {
        if (!url || url.length === 0) {
            setError('Please enter a URL')
            return
        }

        if (!url.startsWith('http://') && !url.startsWith('https://')) {
            setError('Please enter a valid URL')
            return
        }

        if (!localStorage.getItem('auth')) {
            navigate('/login')
            return
        }

        http_common.put("/shorten", {url, expiresAt: null})
            .then(response => {
                setError(null)
                setShortenedUrl(response.data)
            })
            .catch(error => {
                console.error('Error shortening URL', error)
                setError('Error shortening URL')
            })
    }

    const handleCopyClick = async () => {
        await navigator.clipboard.writeText(shortenedUrl)
        setRecentCopied(true)
        setTimeout(() => {
            setRecentCopied(false)
        }, 3000)
    }

    return (
        <Card className="card">
            <h5>
                Shorten Your URL
            </h5>
            <div>
                <TextInput
                    type="text"
                    placeholder="Enter your URL"
                    value={url}
                    onChange={(e) => setUrl(e.target.value)}
                />
                <Button onClick={handleShortenUrl}>Shorten URL</Button>
                {error && <div className="error">{error}</div>}
                {shortenedUrl && (
                    <div className="mt-4 relative">
                        <span>Shortened URL:</span>
                        <a href={shortenedUrl} target="_blank" rel="noopener noreferrer">
                            {shortenedUrl}
                        </a>
                        <HiClipboardCopy
                            className="icon"
                            onClick={handleCopyClick}/>
                    </div>
                )}
                {recentCopied && <div className="success">Copied to clipboard</div>}
            </div>
        </Card>
    )
}
