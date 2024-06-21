import {Outlet} from "react-router-dom"
import Footer from "./Footer.tsx"
import Header from "./Header.tsx"

export default function Layout() {
    return (
        <>
            <Header/>
            <main>
                <Outlet/>
            </main>
            <Footer/>
        </>
    )
}