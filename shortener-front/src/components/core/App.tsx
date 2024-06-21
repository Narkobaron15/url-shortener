import './css/App.css'
import {Route, Routes} from "react-router-dom";
import Layout from "./Layout.tsx";
import HomePage from "../pages/HomePage.tsx";
import NotFound from "../pages/NotFound.tsx";

function App() {
    // TODO: add router
    return (
        <Routes>
            <Route path="/" element={<Layout/>}>
                <Route index element={<HomePage/>}/>
            </Route>
            <Route path="*" element={<NotFound/>}/>
        </Routes>
    )
}

export default App
