import { useEffect, useState } from 'react'
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css'
import { getGames } from './services/applicationsService'

function App() {
    interface GameInfoDTO {
        appid: string;
        name: string;
        type?: string;
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
        <div className="container-fluid min-vh-100 bg-dark text-light py-5">
            {/* Sekcja Hero z Wyszukiwark¹ */}
            <section id="center" className="container mb-5">
                <div className="row justify-content-center w-100">
                    <div className="col-12 col-md-10 col-lg-9">
                        <h1 className="display-4 text-center mb-2 fw-bold text-primary">Steam Discovery</h1>
                        <p className="text-center text-muted mb-5">Find your next favorite game in our database</p>

                        <div className="input-group input-group-lg shadow-lg">
                            <input
                                type="text"
                                className="form-control bg-secondary text-white border-0 ps-4"
                                placeholder="Search by game name (e.g. Witcher, Portal)..."
                            //      value={searchTerm}
                            //    onChange={(e) => setSearchTerm(e.target.value)}
                            //  onKeyDown={(e) => e.key === 'Enter' && handleSearch()}
                            />
                            <button
                                className="btn btn-primary px-5 fw-bold"
                                type="button"
                            // onClick={handleSearch}
                            >
                                <button className="btn btn-primary px-5 fw-bold rounded-pill shadow-sm">
                                    <i className="bi bi-search me-2 fw-bold"></i> Find
                                </button>
                            </button>
                        </div>
                    </div>
                </div>
            </section>

            {/* Sekcja Wyników - Siatka Kart */}
            <div className="container">
                <div className="row g-4">
                    {games.length > 0 ? (
                        games.map((item) => (
                            <div className="col-sm-6 col-md-4 col-lg-3" key={item.appid}>
                                <div className="card h-100 bg-secondary text-white border-0 shadow-sm hover-effect">
                                    {/* Jeœli masz w bazie linki do obrazków, wstaw je w src poni¿ej */}
                                    <div className="card-body d-flex flex-column">
                                        <div className="d-flex justify-content-between align-items-start mb-2">
                                            <span className="badge bg-primary">{item.type}</span>
                                            <small className="text-info">#{item.appid}</small>
                                        </div>
                                        <h5 className="card-title fw-bold mb-3">{item.name}</h5>

                                        <button className="btn btn-outline-light btn-sm mt-auto">
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
