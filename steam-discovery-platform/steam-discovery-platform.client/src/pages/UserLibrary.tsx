import { useState } from 'react';

function UserLibrary() {
    // Zamockowane dane na podstawie Twojej tabeli w bazie
    const games = [
        { appId: 10, title: "Counter-Strike", isFavorite: false, addedAt: "2026-04-08", imgUrl: "https://placehold.co/460x215/1b2838/dcdedf?text=Counter-Strike" },
        { appId: 70, title: "Half-Life", isFavorite: true, addedAt: "2026-03-25", imgUrl: "https://placehold.co/460x215/1b2838/dcdedf?text=Half-Life" },
        { appId: 204360, title: "Castle Crashers", isFavorite: true, addedAt: "2026-03-20", imgUrl: "https://placehold.co/460x215/1b2838/dcdedf?text=Castle+Crashers" },
        { appId: 379720, title: "DOOM", isFavorite: false, addedAt: "2026-02-15", imgUrl: "https://placehold.co/460x215/1b2838/dcdedf?text=DOOM" },
        { appId: 1097840, title: "Gears 5", isFavorite: false, addedAt: "2026-01-10", imgUrl: "https://placehold.co/460x215/1b2838/dcdedf?text=Gears+5" },
    ];

    const [filter, setFilter] = useState('all'); // 'all' lub 'favorites'

    // Filtrowanie gier
    const displayedGames = filter === 'favorites' 
        ? games.filter(g => g.isFavorite) 
        : games;

    return (
        <div className="container-fluid py-5 min-vh-100" style={{ background: '#0f141d', color: '#dcdedf' }}>
            <div className="container">
                
                {/* Nagłówek i Filtry */}
                <div className="d-flex justify-content-between align-items-end border-bottom border-secondary pb-3 mb-4">
                    <div>
                        <h2 className="fw-bold mb-0 text-white"><i className="bi bi-collection-play me-2"></i>My Library</h2>
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

                {/* Siatka Gier */}
                <div className="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
                    {games.map((item) => (

                    <div className="col-sm-6 col-md-4 col-lg-3" key={item.appId}>
                            <div className="card h-100 card-custom text-white border-0 ">
                                {item.isFavorite && (
                                    <button
                                    //    onClick={() => toggleFavorite(game.appId)}
                                        className="btn btn-link position-absolute top-0 end-0 p-2 border-0"
                                        style={{ zIndex: 2, textDecoration: 'none', background: 'transparent' }}
                                        title={item.isFavorite ? "Remove from favorites" : "Add to favorites"}
                                    >
                                        <i className={`bi ${item.isFavorite ? 'bi-heart-fill text-danger' : 'bi-heart text-white'} drop-shadow fs-5 favorite-icon`}></i>
                                    </button>
                                )}
                                {item.imgUrl ? (
                                <img
                                        src={item.imgUrl}
                                    className="card-img-top"
                                    alt={item.title}
                                    style={{ objectFit: 'cover', height: '160px' }}
                                />
                            ) : (
                                <div className="bg-secondary d-flex align-items-center justify-content-center" style={{ height: '160px' }}>
                                    <i className="bi bi-controller fs-1"></i>
                                </div>
                            )}

                            <div className="card-body d-flex flex-column">
                                <div className="d-flex justify-content-between align-items-start mb-2">
                                    <span className="badge bg-danger">{item.addedAt}</span>
                                    <small className="text-light">#{item.appId}</small>
                                </div>
                                <h5 className="card-title fw-bold mb-3">{item.title}</h5>
                                <button className="btn btn-steam-details btn-sm mt-auto"
                                      // onClick={() => navigateToGameDetails(Number(item.appid))}
                                    >
                                    View Details
                                </button>
                                    </div>
                                </div>
                            </div>
                    ))};
                </div>

                {/* Pusty stan dla filtrów */}
                {displayedGames.length === 0 && (
                    <div className="text-center py-5">
                        <i className="bi bi-controller display-1 text-secondary opacity-25 mb-3"></i>
                        <h4 className="text-secondary">No games found</h4>
                        <p className="text-muted">Try changing your filters.</p>
                    </div>
                )}
            </div>
        </div>
    );
}

export default UserLibrary;