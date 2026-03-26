import { useEffect, useState } from 'react'
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import './App.css'
import { getGames } from './services/applicationsService'
function App() {
    interface GameInfoDTO {
        appid: string;
        name: string;
        type?: string;
        headerImage: string;
    }

    const [games, setGame] = useState<GameInfoDTO[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchGames = async () => {
            try {
                const data = await getGames();
                setGame(data);
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchGames();
    }, []);

    if (loading) return <p>Loading...</p>;

    return (
        <div className="container-fluid min-vh-100 main-bg-gradient text-light py-5">
            {/* Sekcja Hero z Wyszukiwarką */}
            <section id="center" className="container mb-5">
                <div className="row justify-content-center w-100">
                    <div className="col-12 col-md-10 col-lg-9">
                        <h1 className="display-4 text-center mb-2 fw-bold text-danger">Steam Discovery</h1>
                        <p className="text-center text-light mb-5 ">Find your next favorite game in our database</p>

                        <div className="input-group input-group-lg shadow-lg">
                            <input
                                type="text"
                                className="form-control bg-secondary text-light border-0 ps-4"
                                placeholder="Search by game name (e.g. Witcher, Portal)..."
                            //      value={searchTerm}
                            //    onChange={(e) => setSearchTerm(e.target.value)}
                            //  onKeyDown={(e) => e.key === 'Enter' && handleSearch()}
                            />
                            <button className="btn btn-danger shadow-sm search-icon">
                                <i className="bi bi-search fw-bold "></i>
                            </button>
                        </div>
                    </div>
                </div>
            </section>

            <div className="container">
                <div className="row g-4">
                    {games.length > 0 ? (
                        games.map((item) => (
                            <div className="col-sm-6 col-md-4 col-lg-3" key={item.appid}>
                                <div className="card h-100 card-custom text-white border-0 shadow-sm hover-effect overflow-hidden">
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
export default App
