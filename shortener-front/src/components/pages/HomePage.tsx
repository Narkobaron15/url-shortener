import {Button, Card, TextInput} from "flowbite-react";
import {useState} from "react";
import './css/HomePage.css';

export default function HomePage() {
    const [url, setUrl] = useState('');
    const [shortenedUrl, setShortenedUrl] = useState('');

    const handleShortenUrl = () => {
        // Placeholder for URL shortening logic
        setShortenedUrl(`short.ly/${url.slice(-6)}`);
    };

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
                {shortenedUrl && (
                    <div className="mt-4">
                        <span>Shortened URL:</span>
                        <a href={"https://" + shortenedUrl} target="_blank" rel="noopener noreferrer">
                            {shortenedUrl}
                        </a>
                    </div>
                )}
            </div>
        </Card>
    )
}
