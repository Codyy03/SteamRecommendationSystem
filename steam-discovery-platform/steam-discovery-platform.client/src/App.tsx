import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './layout/Layout';
import HomePage from './pages/HomePage';
{/* import RecommendationPage from './pages/RecommendationPage';*/}

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route element={<Layout />}>
                    <Route path="/" element={<HomePage />} />
                    {/* <Route path="/recommendations/:gameName" element={<RecommendationPage />} /> */}
                </Route>
            </Routes>
        </BrowserRouter>
    );
}
export default App;