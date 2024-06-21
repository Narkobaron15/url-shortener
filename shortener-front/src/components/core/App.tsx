import './css/App.css'
import {Route, Routes} from "react-router-dom";
import Layout from "./Layout.tsx";
import HomePage from "../pages/HomePage.tsx";
import NotFoundPage from "../pages/NotFoundPage.tsx";
import RegisterPage from "../pages/RegisterPage.tsx";
import LoginPage from "../pages/LoginPage.tsx";

function App() {
    return (
        <Routes>
            <Route path="/" element={<Layout/>}>
                <Route index element={<HomePage/>}/>
                <Route path="login" element={<LoginPage/>}/>
                <Route path="register" element={<RegisterPage/>}/>
                {/* TODO: add route for about page */}
            </Route>
            <Route path="*" element={<NotFoundPage/>}/>
        </Routes>
    )
}

export default App
