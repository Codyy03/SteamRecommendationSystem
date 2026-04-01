import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import "./homePage.css"
import { getGames, getGamesByGenre } from '../services/applicationsService'

let isInitialLoad = true;
function HomePage() {
    interface GameInfoDTO {
        appid: string;
        name: string;
        type?: string;
        headerImage: string;
    }

    const [games, setGame] = useState<GameInfoDTO[]>([]);
    const [activeGenre, setActiveGenre] = useState('');
    const navigate = useNavigate();

    const [loading, setLoading] = useState(true);
    useEffect(() => {
        const fetchGames = async () => {
            if (isInitialLoad) {
                sessionStorage.removeItem('cached_home_games');
                sessionStorage.removeItem('active_genre');
                isInitialLoad = false;
            }

            const cachedGames = sessionStorage.getItem('cached_home_games');
            const cachedGenre = sessionStorage.getItem('active_genre');

            if (cachedGames) {
                setGame(JSON.parse(cachedGames));
                if (cachedGenre) setActiveGenre(cachedGenre);
                setLoading(false);
                return;
            }

            try {
                const data = await getGames();
                setGame(data);
                sessionStorage.setItem('cached_home_games', JSON.stringify(data));
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchGames();
    }, []);

    if (loading) return <p>Loading...</p>;

    const handleGenreSelect = async (genre: string) => {
        try {
            const data = await getGamesByGenre(genre);
            setGame(data);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const handleTagClick = async (genre: string) => {
        setActiveGenre(genre);
        setLoading(true);
        try {
            const data = await getGamesByGenre(genre);
            setGame(data);

            sessionStorage.setItem('cached_home_games', JSON.stringify(data));
            sessionStorage.setItem('active_genre', genre);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const navigateToGameDetails = (id: number) => {
        navigate(`gameInfo/${id}`)
    }

    return (
        <div className="container-fluid min-vh-100 main-bg-gradient text-light">
            <div className="d-flex flex-wrap justify-content-center gap-2 mb-4">
                {['Action', 'RPG', 'Strategy', 'Indie', 'Simulation'].map(genre => (
                    <button
                        key={genre}
                        className={`btn btn-sm rounded-pill px-3 transition-all ${activeGenre === genre ? 'btn-danger' : 'btn-outline-secondary text-light'
                            }`}
                        onClick={() => handleTagClick(genre)}
                        style={{
                            background: activeGenre === genre ? '' : 'rgba(255,255,255,0.05)',
                            border: '1px solid rgba(255,255,255,0.1)'
                        }}
                    >
                        {genre}
                    </button>
                ))}
            </div>
            <div className="container">
                <div className="row g-4">
                    {games.length > 0 ? (
                        games.map((item) => (
                            <div className="col-sm-6 col-md-4 col-lg-3" key={item.appid}>
                                <div className="card h-100 card-custom text-white border-0 ">
                                    {item.headerImage ? (
                                        <img
                                            src={item.headerImage}
                                            className="card-img-top"
                                            alt={item.name}
                                            style={{ objectFit: 'cover', height: '160px' }}
                                        />
                                    ) : (
                                        <div className="bg-secondary d-flex align-items-center justify-content-center" style={{ height: '160px' }}>
                                            <i className="bi bi-controller fs-1"></i>
                                        </div>
                                    )}

                                    <div className="card-body d-flex flex-column">
                                        <div className="d-flex justify-content-between align-items-start mb-2">
                                            <span className="badge bg-danger">{item.type}</span>
                                            <small className="text-light">#{item.appid}</small>
                                        </div>
                                        <h5 className="card-title fw-bold mb-3">{item.name}</h5>
                                        <button className="btn btn-steam-details btn-sm mt-auto"
                                            onClick={() => navigateToGameDetails(Number(item.appid))}>
                                            View Details
                                        </button>
                                    </div>
                                </div>
                            </div>
                        ))
                    ) : (
                        !loading && <div className="text-center text-muted w-100 mt-5">No games found. Try searching for something else!</div>
                    )}
                </div>
            </div>
        </div>
    );
}
export default HomePage
