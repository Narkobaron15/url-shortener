import './css/AboutPage.css'

export default function AboutPage() {
    return (
        <div className="about">
            <h1>About Us</h1>
            <p>
                Welcome to Shorty, your go-to URL shortener app! We make it easy to
                shorten, manage, and track your links for better sharing and analytics.
            </p>
            <h2>Our Mission</h2>
            <p>
                Our mission is to simplify the way you share links online. Whether
                you're a business looking to track marketing campaigns or an individual
                who wants a cleaner way to share URLs, Shorty is here to help.
            </p>
            <h2>Features</h2>
            <ul>
                <li>Easy-to-use URL shortening</li>
                <li>Customizable short links</li>
                <li>Detailed analytics and tracking</li>
                <li>Secure and reliable service</li>
            </ul>
            <h2>Get in Touch</h2>
            <p>
                Have questions or need support? Reach out to us at{' '}
                <a href="mailto:support@shorty.com">
                    support@shorty.com
                </a>
                . We're here to help!
            </p>
        </div>
    )
}
