import {Navbar} from "flowbite-react"
import './css/Header.css'

export default function Header() {
    return (
        <Navbar fluid={true} rounded={true}>
            <Navbar.Brand href="/">
                <img
                    src="https://flowbite.com/docs/images/logo.svg"
                    className="mr-3 h-6 sm:h-9"
                    alt="Flowbite Logo"
                />
                <h1 className="styled">
                  ShorterUrl
                </h1>
            </Navbar.Brand>
            <Navbar.Toggle/>
            <Navbar.Collapse>
                <Navbar.Link href="/" active={true}>
                    Home
                </Navbar.Link>
                <Navbar.Link href="/about">
                    About
                </Navbar.Link>
                {/*<Navbar.Link href="/services">*/}
                {/*    Services*/}
                {/*</Navbar.Link>*/}
                <Navbar.Link href="/account">
                    Account
                </Navbar.Link>
            </Navbar.Collapse>
        </Navbar>
    )
}
