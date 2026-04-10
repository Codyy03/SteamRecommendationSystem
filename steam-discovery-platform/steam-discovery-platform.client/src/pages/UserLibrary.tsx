import { useState, useEffect } from 'react';
import { getUserLibrary } from '../services/userLibraryService';
import { useNavigate } from 'react-router-dom'
function UserLibrary() {
    interface GameInfo {
        appid: number;
        name: string;
        type?: string;
        headerImage?: string;
    }
    interface UserLibraryGame {
        isFavorite: boolean;
        addedAt: string; 
        game: GameInfo;
    }

    const [games, setGames] = useState<UserLibraryGame[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [filter, setFilter] = useState('all');

    useEffect(() => {
        const fetchGames = async () => {
            try {
                setLoading(true);
                const data = await getUserLibrary();
                setGames(data);
            } catch (err) {
                console.error("Failed to load library:", err);
                setError("Could not load your library. Please try again later.");
            } finally {
                setLoading(false);
            }
        };

        fetchGames();
    }, []);

    const displayedGames = filter === 'favorites'
        ? games.filter(item => item.isFavorite)
        : games;

    const navigate = useNavigate();

    const navigateToGameDetails = (id: number) => {
        navigate(`/gameInfo/${id}`)
    }

    if (loading) return (
        <div className="container-fluid py-5 min-vh-100 bg-dark d-flex justify-content-center align-items-center">
            <div className="spinner-border text-danger" role="status"></div>
        </div>
    );

    return (
        <div className="container-fluid py-5 min-vh-100  main-bg-gradient" >
            <div className="container">

                <div className="d-flex justify-content-between align-items-end border-bottom border-secondary pb-3 mb-4">
                    <div>
                        <h2 className="fw-bold mb-0 text-white">
                            <i className="bi bi-collection-play me-2"></i>My Library
                        </h2>
                        <span className="text-secondary small">{games.length} games in total</span>
                    </div>

                    <div className="btn-group shadow-sm">
                        <button
                            className={`btn btn-sm ${filter === 'all' ? 'btn-secondary' : 'btn-outline-secondary'}`}
                            onClick={() => setFilter('all')}>
                            All Games
                        </button>
                        <button
                            className={`btn btn-sm ${filter === 'favorites' ? 'btn-danger' : 'btn-outline-danger'}`}
                            onClick={() => setFilter('favorites')}>
                            <i className="bi bi-heart-fill me-1"></i> Favorites
                        </button>
                    </div>
                </div>

                {error && <div className="alert alert-danger">{error}</div>}

                {/* games grid */}
                <div className="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
                    {displayedGames.map((item) => (
                        <div className="col" key={item.game.appid}>
                            <div className="card h-100 card-custom text-white border-0 position-relative"
                                style={{ backgroundColor: '#1b2838', borderRadius: '10px', overflow: 'hidden' }}>

                                {/* Favorite Icon */}
                                <button className="btn btn-link position-absolute top-0 end-0 p-2 border-0" style={{ zIndex: 2 }}>
                                    <i className={`bi ${item.isFavorite ? 'bi-heart-fill text-danger' : 'bi-heart text-white'} fs-5`}></i>
                                </button>

                                {/* Game Image */}
                                {item.game.headerImage ? (
                                    <img
                                        src={item.game.headerImage}
                                        className="card-img-top"
                                        alt={item.game.name}
                                        style={{ objectFit: 'cover', height: '160px' }}
                                    />
                                ) : (
                                    <div className="bg-secondary d-flex align-items-center justify-content-center" style={{ height: '160px' }}>
                                        <i className="bi bi-controller fs-1"></i>
                                    </div>
                                )}

                                <div className="card-body d-flex flex-column">
                                    <div className="d-flex justify-content-between align-items-start mb-2 bg-transparent">
                                       
                                        <span className="badge bg-secondary text-light border border-secondary">
                                            {new Date(item.addedAt).toLocaleDateString()}
                                        </span>
                                        <small className="text-light">#{item.game.appid}</small>
                                    </div>
                                    <h5 className="card-title fw-bold mb-3 text-truncate" title={item.game.name}>
                                        {item.game.name}
                                    </h5>
                                    <button className="btn btn-outline-danger btn-sm mt-auto fw-bold"
                                        onClick={() => navigateToGameDetails(item.game.appid)}>
                                        View Details
                                    </button>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>

                {/* no games*/}
                {displayedGames.length === 0 && !loading && (
                    <div className="text-center py-5">
                        <i className="bi bi-controller display-1 text-light opacity-25 mb-3"></i>
                        <h4 className="text-light">No games found</h4>
                        <p className="text-light">Your library is empty or matches no filters.</p>
                    </div>
                )}
            </div>
        </div>
    );
}

export default UserLibrary;