import './App.css'
import {Route, Routes} from "react-router-dom";

function App() {
    // TODO: add router
    return (
        <Routes>
            <Route path="/">
                <Route index element={<HomePage/>}/>
            </Route>
            <Route path="*" element={<NotFound/>}/>
        </Routes>
    )
}

export default App
