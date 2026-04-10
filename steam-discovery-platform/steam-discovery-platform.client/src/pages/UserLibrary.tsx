import { useState, useEffect, useMemo } from 'react';
import { getUserLibrary } from '../services/userLibraryService';
import { useNavigate } from 'react-router-dom'
import api from '../services/api';
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
        genres: string;
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

    const navigate = useNavigate();

    const navigateToGameDetails = (id: number) => {
        navigate(`/gameInfo/${id}`)
    }

    const [searchTerm, setSearchTerm] = useState('');
    const [sortBy, setSortBy] = useState('newest');
    const [selectedGenre, setSelectedGenre] = useState('all'); // NOWY STAN

    const availableGenres = useMemo(() => {
        const genresSet = new Set<string>();
        games.forEach(item => {
            if (item.genres) {
                // Rozdzielamy string "Action, RPG" na pojedyncze s³owa i usuwamy spacje
                item.genres.split(',').forEach((g: string) => genresSet.add(g.trim()));
            }
        });
        // Zwracamy posortowan¹ alfabetycznie tablicê gatunków
        return Array.from(genresSet).sort();
    }, [games]);

    const displayedGames = games
        .filter(item => {
            const matchesFilter = filter === 'all' || item.isFavorite;
            const matchesSearch = item.game.name.toLowerCase().includes(searchTerm.toLowerCase());
            const matchesGenre = selectedGenre === 'all' || (item.genres && item.genres.includes(selectedGenre));

            return matchesFilter && matchesSearch && matchesGenre;
        })
        .sort((a, b) => {
            if (sortBy === 'newest') return new Date(b.addedAt).getTime() - new Date(a.addedAt).getTime();
            if (sortBy === 'oldest') return new Date(a.addedAt).getTime() - new Date(b.addedAt).getTime();
            if (sortBy === 'az') return a.game.name.localeCompare(b.game.name);
            return 0;
        });


    const handleGetRecommendations = async () => {
        // displayedGames ju¿ uwzglêdnia wyszukiwarkê, zak³adkê ulubione i gatunek!
        const gameIdsToSend = displayedGames.map(item => item.game.appid);

        if (gameIdsToSend.length === 0) {
            alert("No games to base recommendations on. Change your filters!");
            return;
        }

        // Wyœlij gameIdsToSend do Pythona
    }

    const handleRemoveGame = async (appid: number) => {
        try {
            await api.delete(`/api/usersLibrary/delete/${appid}`);

            setGames(prevGames => prevGames.filter(item => item.game.appid !== appid));
        } catch (error) {
            console.error("Failed to remove game:", error);
        }
    }

    if (loading) return (
        <div className="container-fluid py-5 min-vh-100 bg-dark d-flex justify-content-center align-items-center">
            <div className="spinner-border text-danger" role="status"></div>
        </div>
    );

    return (
        <div className="container-fluid py-5 min-vh-100  main-bg-gradient" >
            <div className="container">
                {/* NAG£ÓWEK I REKOMENDACJE */}
                <div className="d-flex justify-content-between align-items-center mb-4">
                    <div>
                        <h2 className="fw-bold mb-0 text-white">
                            <i className="bi bi-collection-play me-2"></i>My Library
                        </h2>
                        <span className="text-secondary small">{games.length} games in total</span>
                    </div>

                    {/* PRZYCISK REKOMENDACJI (Python API) */}
                    <button className="btn btn-outline-info d-flex align-items-center gap-2 shadow-sm fw-bold">
                        <i className="bi bi-magic"></i>
                        <span>Get AI Recommendations</span>
                    </button>
                </div>

                {/* TOOLBAR Z FILTRAMI */}
                <div className="row g-3 mb-4 bg-dark p-3 rounded-3 shadow-sm border border-secondary">

                    {/* 1. Wyszukiwarka */}
                    <div className="col-12 col-md-6 col-lg-3">
                        <div className="input-group">
                            <span className="input-group-text bg-black border-secondary text-secondary">
                                <i className="bi bi-search"></i>
                            </span>
                            <input
                                type="text"
                                className="form-control bg-black border-secondary text-white shadow-none"
                                placeholder="Search by title..."
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                        </div>
                    </div>

                    {/* 2. Filtrowanie po Gatunku */}
                    <div className="col-12 col-md-6 col-lg-3">
                        <div className="input-group">
                            <span className="input-group-text bg-black border-secondary text-secondary">
                                <i className="bi bi-tags"></i>
                            </span>
                            <select
                                className="form-select bg-black border-secondary text-white shadow-none"
                                value={selectedGenre}
                                onChange={(e) => setSelectedGenre(e.target.value)}
                            >
                                <option value="all">All Genres</option>
                                {availableGenres.map(genre => (
                                    <option key={genre} value={genre}>{genre}</option>
                                ))}
                            </select>
                        </div>
                    </div>

                    {/* 3. Sortowanie */}
                    <div className="col-12 col-md-6 col-lg-3">
                        <div className="input-group">
                            <span className="input-group-text bg-black border-secondary text-secondary">
                                <i className="bi bi-sort-down"></i>
                            </span>
                            <select
                                className="form-select bg-black border-secondary text-white shadow-none"
                                value={sortBy}
                                onChange={(e) => setSortBy(e.target.value)}
                            >
                                <option value="newest">Newest Added</option>
                                <option value="oldest">Oldest Added</option>
                                <option value="az">Name (A-Z)</option>
                            </select>
                        </div>
                    </div>

                    {/* 4. Zak³adki (All/Favorites) */}
                    <div className="col-12 col-md-6 col-lg-3 d-flex justify-content-md-end align-items-center">
                        <div className="btn-group w-100">
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
                </div>

                {error && <div className="alert alert-danger">{error}</div>}

                {/* games grid */}
                <div className="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
                    {displayedGames.map((item) => (
                        <div className="col" key={item.game.appid}>
                            <div className="card h-100 card-custom text-white border-0 position-relative"
                                style={{ backgroundColor: '#1b2838', borderRadius: '10px', overflow: 'hidden' }}>

                                {/* Favorite Icon */}
                                <button className="btn btn-link position-absolute top-0 end-0 p-2 border-0" style={{ zIndex: 2 }}
                                    onClick={() => handleGetRecommendations()}>
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
                                    <div className="mt-auto d-flex gap-2">
                                        <button
                                            className="btn btn-outline-danger btn-sm flex-grow-1 fw-bold"
                                            onClick={() => navigateToGameDetails(item.game.appid)}>
                                            View Details
                                        </button>
                                        <button
                                            className="btn btn-outline-secondary btn-sm px-3"
                                            title="Remove from library"
                                         onClick={() => handleRemoveGame(item.game.appid)}>
                                            <i className="bi bi-trash3"></i>
                                        </button>
                                    </div>
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