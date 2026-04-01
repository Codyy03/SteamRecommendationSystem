import { useState, useEffect } from 'react';
import { useParams} from 'react-router-dom';
import { getGameDetails } from '../services/applicationsService';
function GameDetailsPage() {
    interface GameDetailsDTO {
        appid: number;
        name: string;
        type: string;
        isFree: boolean;
        releaseDate: Date;
        shortDescription: string;
        headerImage: string;
        metacriticScore: string;
        recommendationsTotal: number;
        finalPrice: number;
        currency: string;
        supportsWindows: boolean;
        supportsMac: boolean;
        supportsLinux: boolean;
        pcRequirements: string;
        createdAt: Date;
        developers: string;
        publishers: string;
        categories: string;
        genres: string;
    }

    const [gameDetails, setGameDetails] = useState<GameDetailsDTO | null>();
    const [loading, setLoading] = useState(true);
    const { id } = useParams<{ id: string }>();

    useEffect(() => {
        if (!id) return;

        const fetchData = async (id: number) => {
            try {
                const data = await getGameDetails(id);
                setGameDetails(data);
            } catch (err) {
                console.log(err);
            } finally {
                setLoading(false);
            }
        };
        fetchData(Number(id));
    }, [id]);

    const requirements = typeof gameDetails?.pcRequirements === 'string'
        ? JSON.parse(gameDetails.pcRequirements)
        : gameDetails?.pcRequirements;

    const min = requirements?.minimum || {};
    const rec = requirements?.recommended || {};

    if (loading) {
        return (
            <div className="container-fluid min-vh-100 main-bg-gradient d-flex align-items-center justify-content-center">
                <div className="loader-container">
                    <div className="spinner-border spinner-steam mb-3" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="container-fluid min-vh-100 main-bg-gradient py-5 px-4">
            {gameDetails && (
                <div className="container py-4">
                    {/* title */}
                    <div className="d-flex align-items-center mb-4">
                        <button onClick={() => window.history.back()} className="btn btn-outline-danger me-3 btn-sm">
                            <i className="bi bi-arrow-left"></i>
                        </button>
                        <h1 className="text-light fw-bold mb-0">{gameDetails.name}</h1>
                    </div>

                    <div className="row g-4">
                        {/* image and description */}
                        <div className="col-lg-8">
                            <div className="card bg-dark border-0 shadow-lg overflow-hidden" style={{ borderRadius: '15px' }}>
                                <img
                                    src={gameDetails.headerImage}
                                    className="img-fluid w-100"
                                    alt={gameDetails.name}
                                    style={{ maxHeight: '400px', objectFit: 'cover' }}
                                />
                                <div className="card-body p-4">
                                    <h4 className="text-danger mb-3">About the {gameDetails.type}</h4>
                                    <p className="text-light opacity-75 lh-lg">
                                        {gameDetails.shortDescription}
                                    </p>
                                </div>
                            </div>
                            <div className="card border-0 shadow-lg mt-4" style={{ background: 'rgba(30, 34, 45, 0.4)', borderRadius: '15px' }}>
                                <div className="card-body p-4 text-light">
                                    <h4 className="text-danger mb-4"><i className="bi bi-cpu me-2"></i>System Requirements</h4>

                                    <div className="row g-5">
                                        {/* MINIMUM */}
                                        <div className="col-md-6 border-end border-secondary border-opacity-10">
                                            <h6 className="text-uppercase fw-bold mb-3 opacity-50 small" style={{ letterSpacing: '1px' }}>Minimum</h6>
                                            <div className="d-flex flex-column gap-3">
                                                <div className="requirement-item">
                                                    <small className="text-danger d-block mb-1">OS</small>
                                                    <span className="small">{min.os || 'N/A'}</span>
                                                </div>
                                                <div className="requirement-item">
                                                    <small className="text-danger d-block mb-1">Processor</small>
                                                    <span className="small">{min.processor || 'N/A'}</span>
                                                </div>
                                                <div className="requirement-item">
                                                    <small className="text-danger d-block mb-1">Memory</small>
                                                    <span className="small">{min.memory || 'N/A'}</span>
                                                </div>
                                                <div className="requirement-item">
                                                    <small className="text-danger d-block mb-1">Graphics</small>
                                                    <span className="small">{min.graphics || 'N/A'}</span>
                                                </div>
                                            </div>
                                        </div>

                                        {/* RECOMMENDED */}
                                        <div className="col-md-6">
                                            <h6 className="text-uppercase fw-bold mb-3 opacity-50 small" style={{ letterSpacing: '1px' }}>Recommended</h6>
                                            <div className="d-flex flex-column gap-3">
                                                <div className="requirement-item">
                                                    <small className="text-danger d-block mb-1">OS</small>
                                                    <span className="small">{rec.os || min.os || 'N/A'}</span>
                                                </div>
                                                <div className="requirement-item">
                                                    <small className="text-danger d-block mb-1">Processor</small>
                                                    <span className="small">{rec.processor || 'N/A'}</span>
                                                </div>
                                                <div className="requirement-item">
                                                    <small className="text-danger d-block mb-1">Memory</small>
                                                    <span className="small">{rec.memory || rec.memory || 'N/A'}</span>
                                                </div>
                                                <div className="requirement-item">
                                                    <small className="text-danger d-block mb-1">Graphics</small>
                                                    <span className="small">{rec.graphics || 'N/A'}</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div className="col-lg-4">
                            <div className="card border-0 shadow-lg text-light p-4"
                                style={{
                                    background: 'rgba(30, 34, 45, 0.6)',
                                    backdropFilter: 'blur(10px)',
                                    borderRadius: '15px',
                                    position: 'sticky',
                                    top: '20px'
                                }}>

                                {/* PRICE SECTION */}
                                <div className="mb-4">
                                    <div className="d-flex justify-content-between align-items-center mb-2">
                                        <span className="small opacity-50">Price</span>
                                        <span className="fs-3 fw-bold text-danger">
                                            {gameDetails.isFree ? 'FREE' : `${(gameDetails.finalPrice / 100).toFixed(2)} ${gameDetails.currency}`}
                                        </span>
                                    </div>
                                    <button className="btn btn-danger w-100 fw-bold py-2 shadow-sm border-0 mb-2">
                                        ADD TO FAVORITE
                                    </button>
                                </div>

                                <hr className="border-secondary opacity-25" />

                                {/* QUICK STATS */}
                                <div className="my-4 d-flex flex-column gap-2">
                                    <div className="d-flex justify-content-between">
                                        <span className="small opacity-50"><i className="bi bi-star-fill me-2"></i>Metacritic</span>
                                        <span className={`badge ${parseInt(gameDetails.metacriticScore) > 75 ? 'bg-success' : 'bg-secondary'}`}>
                                            {gameDetails.metacriticScore || 'N/A'}
                                        </span>
                                    </div>
                                    <div className="d-flex justify-content-between">
                                        <span className="small opacity-50"><i className="bi bi-hand-thumbs-up-fill me-2"></i>Reviews</span>
                                        <span className="fw-bold text-end">{gameDetails.recommendationsTotal.toLocaleString()}</span>
                                    </div>
                                    <div className="d-flex justify-content-between">
                                        <span className="small opacity-50"><i className="bi bi-calendar-event me-2"></i>Release</span>
                                        <span className="fw-bold text-end">{new Date(gameDetails.releaseDate).toLocaleDateString()}</span>
                                    </div>
                                </div>

                                <hr className="border-secondary opacity-25" />

                                {/* PLATFORMS */}
                                <div className="mb-4">
                                    <p className="small opacity-50 mb-2">Available on:</p>
                                    <div className="d-flex gap-3 fs-4">
                                        {gameDetails.supportsWindows && <i className="bi bi-windows text-light" title="Windows"></i>}
                                        {gameDetails.supportsMac && <i className="bi bi-apple text-light" title="Mac"></i>}
                                        {gameDetails.supportsLinux && <i className="bi bi-ubuntu text-light" title="Linux"></i>}
                                    </div>
                                </div>

                                <hr className="border-secondary opacity-25" />

                                {/* TAGS & PEOPLE (Split Strings) */}
                                <div className="d-flex flex-column gap-4">
                                    {/* Developers */}
                                    <div>
                                        <span className="small opacity-50 d-block mb-2 text-uppercase" style={{ fontSize: '0.7rem', letterSpacing: '1px' }}>Developer</span>
                                        <div className="d-flex flex-wrap gap-1">
                                            {gameDetails.developers?.split(', ').map((dev, i) => (
                                                <span key={i} className="badge bg-dark border border-secondary fw-normal px-2 py-1">
                                                    {dev}
                                                </span>
                                            ))}
                                        </div>
                                    </div>

                                    {/* Publishers */}
                                    <div>
                                        <span className="small opacity-50 d-block mb-2 text-uppercase" style={{ fontSize: '0.7rem', letterSpacing: '1px' }}>Publisher</span>
                                        <div className="d-flex flex-wrap gap-1">
                                            {gameDetails.publishers?.split(', ').map((pub, i) => (
                                                <span key={i} className="badge bg-dark border border-secondary fw-normal px-2 py-1">
                                                    {pub}
                                                </span>
                                            ))}
                                        </div>
                                    </div>
                                    {/* Genres */}
                                    <div>
                                        <span className="small opacity-50 d-block mb-2 text-uppercase" style={{ fontSize: '0.7rem', letterSpacing: '1px' }}>Gneres</span>
                                        <div className="d-flex flex-wrap gap-1">
                                            {gameDetails.genres?.split(', ').map((pub, i) => (
                                                <span key={i} className="badge bg-dark border border-secondary fw-normal px-2 py-1 text-warning">
                                                    {pub}
                                                </span>
                                            ))}
                                        </div>
                                    </div>
                                    {/* Categories */}
                                    <div>
                                        <span className="small opacity-50 d-block mb-2 text-uppercase" style={{ fontSize: '0.7rem', letterSpacing: '1px' }}>Categories</span>
                                        <div className="d-flex flex-wrap gap-1">
                                            {gameDetails.categories?.split(', ').map((cat, i) => (
                                                <span key={i} className="badge border border-danger text-danger fw-normal" style={{ fontSize: '0.75rem' }}>
                                                    {cat}
                                                </span>
                                            ))}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default GameDetailsPage