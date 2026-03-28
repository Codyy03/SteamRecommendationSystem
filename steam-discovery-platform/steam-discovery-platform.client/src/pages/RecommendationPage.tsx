import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { getPythonRecomentationGamesByName } from '../services/pythonRecommendationService';

function RecommendationPage() {
    interface GameInfoDTO {
        appid: string;
        name: string;
        type?: string;
        headerImage: string;
    }
    const [games, setGame] = useState<GameInfoDTO[]>([]);

    const [loading, setLoading] = useState(true);
    const { gameName } = useParams();

    useEffect(() => {
        window.scrollTo(0, 0);
        const fetchGames = async () => {
            try {
                const data = await getPythonRecomentationGamesByName(String(gameName));
                setGame(data);
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchGames();
    }, [gameName]);

    if (loading) return <p>Loading...</p>;

    return (
        <div className="container-fluid min-vh-100 main-bg-gradient text-light">
            <div className="container">
                <h2 className="mb-4 text-center ">Recommended for: <span className="text-danger">{gameName}</span></h2>

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