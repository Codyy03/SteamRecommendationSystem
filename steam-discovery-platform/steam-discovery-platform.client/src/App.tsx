import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './layout/Layout';
import HomePage from './pages/HomePage';
import RecommendationPage from './pages/RecommendationPage';
import GameDetailsPage from './pages/GameDetailsPage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route element={<Layout />}>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/recommendations/:gameName" element={<RecommendationPage />} />
                    <Route path="/gameInfo/:id" element={<GameDetailsPage />} />

                </Route>
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/registration" element={<RegisterPage />} />
            </Routes>
        </BrowserRouter>
    );
}
export default App;