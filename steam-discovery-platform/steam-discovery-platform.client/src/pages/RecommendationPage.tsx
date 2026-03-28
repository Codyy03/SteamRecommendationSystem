import { useState, useEffect, useMemo } from 'react';
import { debounce } from 'lodash';
import { useParams } from 'react-router-dom';
import { getPythonRecomentationGamesByName } from '../services/pythonRecommendationService';
import './recommendationPage.css'

function RecommendationPage() {
    interface GameInfoDTO {
        appid: string;
        name: string;
        type?: string;
        headerImage: string;
    }
    const [games, setGame] = useState<GameInfoDTO[]>([]);

    const [loading, setLoading] = useState(true);
    const [isUpdating, setIsUpdating] = useState(false);

    const { gameName } = useParams();

    const [weights, setWeights] = useState({
        genre: 0.4,
        meta: 0.3,
        pop: 0.15
    });

    const fetchGames = async (name: string, w: typeof weights) => {
        try {
            const data = await getPythonRecomentationGamesByName(
                name,
                w.genre,
                w.meta,
                w.pop
            );
            setGame(data);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
            setIsUpdating(false);
        }
    };

    const debouncedFetch = useMemo(
        () => debounce((name: string, w: typeof weights) => {
            fetchGames(name, w);
        }, 500),
        []
    );

    useEffect(() => {
        if (!gameName) return;

        window.scrollTo(0, 0);

        if (games.length === 0) {
            setLoading(true);
        } else {
            setIsUpdating(true);
        }

        debouncedFetch(gameName, weights);

        return () => debouncedFetch.cancel();
    }, [gameName, weights, debouncedFetch]);

    if (loading && games.length === 0) {
        return (
            <div className="container-fluid min-vh-100 main-bg-gradient d-flex align-items-center justify-content-center">
                <div className="loader-container">
                    <div className="spinner-border spinner-steam mb-3" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                    <h3 className="text-light fw-light">Searching for the best matches...</h3>
                </div>
            </div>
        );
    }

    return (
        <div className={`container-fluid min-vh-100 main-bg-gradient text-light ${isUpdating ? 'opacity-75' : ''}`}>
            <div className="container mb-5">
                <div className="card custom-filter-card text-light border-0 shadow-lg">
                    <div className="card-body p-4">
                        <h5 className="card-title mb-4"><i className="bi bi-sliders2-vertical me-2"></i> Recommendation Engine Settings</h5>
                        <div className="row">
                            {/* Genre */}
                            <div className="col-md-4 mb-3 mb-md-0">
                                <label className="form-label d-flex justify-content-between">
                                    Genre Importance <span>{Math.round(weights.genre * 100)}%</span>
                                </label>
                                <input
                                    type="range" className="form-range custom-range"
                                    min="0" max="1" step="0.05" value={weights.genre}
                                    onChange={(e) => setWeights({ ...weights, genre: parseFloat(e.target.value) })}
                                />
                            </div>
                            {/* Metacritic */}
                            <div className="col-md-4 mb-3 mb-md-0">
                                <label className="form-label d-flex justify-content-between">
                                    Rating (Metacritic) <span>{Math.round(weights.meta * 100)}%</span>
                                </label>
                                <input
                                    type="range" className="form-range"
                                    min="0" max="1" step="0.05" value={weights.meta}
                                    onChange={(e) => setWeights({ ...weights, meta: parseFloat(e.target.value) })}
                                />
                            </div>
                            {/* popularity */}
                            <div className="col-md-4">
                                <label className="form-label d-flex justify-content-between">
                                    Popularity <span>{Math.round(weights.pop * 100)}%</span>
                                </label>
                                <input
                                    type="range" className="form-range"
                                    min="0" max="1" step="0.05" value={weights.pop}
                                    onChange={(e) => setWeights({ ...weights, pop: parseFloat(e.target.value) })}
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div className="container">
                <h2 className="mb-4 text-center ">Recommended for: <span className="text-danger">{gameName}</span></h2>
                <div style={{ height: '30px' }} className="mb-3 d-flex justify-content-center align-items-center">
                    {isUpdating && (
                        <div className="d-flex align-items-center text-danger">
                            <div className="spinner-border spinner-border-sm me-2" role="status"></div>
                            <small className="fw-bold text-uppercase italic">Adjusting weights...</small>
                        </div>
                    )}
                </div>
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

                                        <button className="btn btn-steam-details btn-sm mt-auto">
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
export default RecommendationPage;