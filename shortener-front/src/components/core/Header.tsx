import {Navbar} from "flowbite-react"
import './css/Header.css'
import http_common from "../../common/http_common.ts";
import {useNavigate} from "react-router-dom";

export default function Header() {
    const navigate = useNavigate()

    const handleLogout = () => {
        localStorage.removeItem('auth')
        http_common.post('user/logout').then(
            () => {
                http_common.defaults.headers.common.Authorization = ''
                navigate('/login')
            }
        ).catch(console.error)

        window.location.reload()
    }
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
                {localStorage.getItem('auth') &&
                    <Navbar.Link href="#" onClick={handleLogout}>
                        Logout
                    </Navbar.Link>
                }
            </Navbar.Collapse>
        </Navbar>
    )
}
