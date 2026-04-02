import { useState, useEffect, useMemo } from 'react';
import { debounce } from 'lodash';
import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom'
import { getPythonRecomentationGamesByName } from '../services/pythonRecommendationService';
import './recommendationPage.css'

function RecommendationPage() {
    interface PythonRecommendationResponse {
        is_cold_start: boolean;
        base_game: string;
        recommendations: Array<GameInfoDTO>;
    }

    interface GameInfoDTO {
        appid: string;
        name: string;
        type?: string;
        headerImage: string;
    }
    const [games, setGame] = useState<GameInfoDTO[]>([]);
    const [pythonInfo, setPythonInfo] = useState<PythonRecommendationResponse>();

    const [loading, setLoading] = useState(true);
    const [isUpdating, setIsUpdating] = useState(false);

    const { gameName } = useParams();

    const [weights, setWeights] = useState({
        genre: 0.4,
        meta: 0.3,
        pop: 0.15,
        howManyGames: 20,
        series_penalty: 0.4
    });

    const fetchGames = async (name: string, w: typeof weights) => {
        try {
            const data = await getPythonRecomentationGamesByName(
                name,
                w.genre,
                w.meta,
                w.pop,
                w.howManyGames,
                w.series_penalty
            );
            setPythonInfo(data);
            setGame(data?.recommendations || []);
            console.log(String(data?.base_game))
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

    const [innerSearch, setInnerSearch] = useState("");
    const [sortType, setSortType] = useState("relevance");

    const filteredGames = useMemo(() => {
        let result = [...games];

        if (innerSearch) {
            result = result.filter(g =>
                g.name.toLowerCase().includes(innerSearch.toLowerCase())
            );
        }

        if (sortType === "az") {
            result.sort((a, b) => a.name.localeCompare(b.name));
        } else if (sortType === "za") {
            result.sort((a, b) => b.name.localeCompare(a.name));
        }

        return result;
    }, [games, innerSearch, sortType]);

    const navigate = useNavigate();
    const navigateToGameDetails = (id: number) => {
        navigate(`/gameInfo/${id}`);
    }

    const [isExpanded, setIsExpanded] = useState(false);

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
            <div className="container-fluid px-4">
                <div className="container mb-5">
                    <div className="card custom-filter-card text-light border-0 shadow-lg" style={{ background: 'rgba(30, 34, 45, 0.8)', backdropFilter: 'blur(10px)' }}>
                        <div className="card-body p-0">
                            <div
                                className="p-4 d-flex justify-content-between align-items-center"
                                style={{ cursor: 'pointer' }}
                                onClick={() => setIsExpanded(!isExpanded)}
                            >
                                <h5 className="card-title mb-0">
                                    <i className="bi bi-sliders2-vertical me-2 text-danger"></i>
                                    Recommendation Engine Settings
                                </h5>
                                <i className={`bi bi-chevron-down transition-all ${isExpanded ? 'rotate-180' : ''}`}></i>
                            </div>

                            <div className={`filter-collapse ${isExpanded ? 'expanded' : ''}`}>
                                <div className="px-4 pb-4">

                                    {/* weights */}
                                    <div className="row g-4">
                                        <div className="col-md-4">
                                            <label className="form-label d-flex justify-content-between small opacity-75">
                                                Genre Importance <span>{Math.round(weights.genre * 100)}%</span>
                                            </label>
                                            <input
                                                type="range" className="form-range custom-range"
                                                min="0" max="1" step="0.05" value={weights.genre}
                                                onChange={(e) => setWeights({ ...weights, genre: parseFloat(e.target.value) })}
                                            />
                                        </div>
                                        <div className="col-md-4">
                                            <label className="form-label d-flex justify-content-between small opacity-75">
                                                Rating (Metacritic) <span>{Math.round(weights.meta * 100)}%</span>
                                            </label>
                                            <input
                                                type="range" className="form-range custom-range"
                                                min="0" max="1" step="0.05" value={weights.meta}
                                                onChange={(e) => setWeights({ ...weights, meta: parseFloat(e.target.value) })}
                                            />
                                        </div>
                                        <div className="col-md-4">
                                            <label className="form-label d-flex justify-content-between small opacity-75">
                                                Popularity <span>{Math.round(weights.pop * 100)}%</span>
                                            </label>
                                            <input
                                                type="range" className="form-range custom-range"
                                                min="0" max="1" step="0.05" value={weights.pop}
                                                onChange={(e) => setWeights({ ...weights, pop: parseFloat(e.target.value) })}
                                            />
                                        </div>
                                    </div>

                                    {/* Limes (border) */}
                                    <hr className="my-4 border-secondary opacity-25" />

                                    <div className="py-1">
                                        {/* Top Row: Search and Sort */}
                                        <div className="row g-3 align-items-center justify-content-center mb-4">
                                            <div className="col-lg-5 col-md-6">
                                                <label className="form-label small opacity-75">Filter results by name</label>
                                                <div className="input-group">
                                                    <span className="input-group-text bg-dark border-secondary text-secondary">
                                                        <i className="bi bi-search"></i>
                                                    </span>
                                                    <input
                                                        type="text"
                                                        className="form-control bg-dark text-light border-secondary shadow-none"
                                                        placeholder="Search in recommendations..."
                                                        onChange={(e) => setInnerSearch(e.target.value)}
                                                    />
                                                </div>
                                            </div>
                                            <div className="col-lg-5 col-md-6">
                                                <label className="form-label small opacity-75">Sort by</label>
                                                <select
                                                    className="form-select bg-dark text-light border-secondary shadow-none"
                                                    onChange={(e) => setSortType(e.target.value)}
                                                >
                                                    <option value="relevance">Relevance</option>
                                                    <option value="az">Name: A-Z</option>
                                                    <option value="za">Name: Z-A</option>
                                                </select>
                                            </div>
                                        </div>


                                        {/* Bottom row: Side-by-side sliders */}
                                        <div className="row g-4 justify-content-center">
                                            {/* Container: Number of games */}
                                            <div className="col-lg-5 col-md-6">
                                                <div className="d-flex justify-content-between align-items-center mb-2">
                                                    <label className="form-label small opacity-75 mb-0">How many games</label>
                                                    <span className="badge bg-danger opacity-75">{weights.howManyGames}</span>
                                                </div>
                                                <input
                                                    type="range"
                                                    className="form-range custom-range"
                                                    min="10" max="50" step="1"
                                                    value={weights.howManyGames}
                                                    onChange={(e) => setWeights({ ...weights, howManyGames: parseInt(e.target.value) })}
                                                />
                                            </div>

                                            {/* Container: Streak Penalty */}
                                            <div className="col-lg-5 col-md-6">
                                                <div className="d-flex justify-content-between align-items-center mb-2">
                                                    <label className="form-label small opacity-75 mb-0">Penalty for same series</label>
                                                </div>
                                                <input
                                                    type="range"
                                                    className="form-range custom-range"
                                                    min="0" max="1" step="0.05"
                                                    value={weights.series_penalty}
                                                    onChange={(e) => setWeights({ ...weights, series_penalty: parseFloat(e.target.value) })}
                                                />
                                            </div>
                                        </div>
                                    </div>


                                    <div className="mt-3 pt-2 d-flex justify-content-end border-top border-secondary border-opacity-10">
                                        <small className="opacity-50">
                                            Found: <span className="text-danger fw-bold">{filteredGames.length} games</span>
                                        </small>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                {/* results*/}
                <div className="container">
                    <div className="text-center mb-5">
                        {!pythonInfo?.is_cold_start ? < h2 className="fw-bold">Recommended for: <span className="text-danger">{pythonInfo?.base_game}</span></h2>
                            : <h3 className=" text-danger">
                                Game '{pythonInfo?.base_game}' not found, generic matches:
                            </h3>}
                                    
                        <div style={{ height: '30px' }} className="mt-2 d-flex justify-content-center align-items-center">
                            {isUpdating && (
                                <div className="d-flex align-items-center text-danger">
                                    <div className="spinner-border spinner-border-sm me-2" role="status"></div>
                                    <small className="fw-bold text-uppercase italic">Updating Engine...</small>
                                </div>
                            )}
                        </div>
                    </div>

                    <div className="row g-4">
                        {filteredGames.length > 0 ? (
                            filteredGames.map((item) => (
                                <div className="col-sm-6 col-md-4 col-lg-3" key={item.appid}>
                                    <div className="card h-100 card-custom text-white border-0 shadow-sm">
                                        {item.headerImage ? (
                                            <img
                                                src={item.headerImage}
                                                className="card-img-top"
                                                alt={item.name}
                                                style={{ objectFit: 'cover', height: '160px' }}
                                            />
                                        ) : (
                                            <div className="bg-dark d-flex align-items-center justify-content-center" style={{ height: '160px' }}>
                                                <i className="bi bi-controller fs-1 opacity-25"></i>
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
                            !loading && (
                                <div className="col-12 text-center py-5">
                                    <i className="bi bi-exclamation-circle fs-1 text-muted opacity-25 mb-3 d-block"></i>
                                    <h5 className="text-light">No games match your current filters.</h5>
                                    <p className="small text-secondary">Try adjusting the search or sliders.</p>
                                </div>
                            )
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}
export default RecommendationPage;