import { Link } from 'react-router-dom';

function Footer() {
    return (
        <footer className="py-5 mt-auto" >
            <div className="container">
                <div className="row gy-4">

                    <div className="col-12 col-md-5">
                        <h5 className="fw-bold text-white mb-3 d-flex align-items-center">
                            <i className="bi bi-controller fs-4 me-2 text-danger"></i>
                            GameRecommender
                        </h5>
                        <p className="text-secondary small pe-md-5">
                            Your personal game library and intelligent AI-powered recommendation system. Discover new titles, manage your favorites, and build your own collection.
                        </p>
                    </div>

                    <div className="col-6 col-md-3">
                        <h6 className="fw-bold text-white mb-3">Navigation</h6>
                        <ul className="list-unstyled mb-0">
                            <li className="mb-2">
                                <Link to="/" className="text-secondary text-decoration-none">
                                    <i className="bi bi-chevron-right small me-1"></i> Home
                                </Link>
                            </li>
                            <li className="mb-2">
                                <Link to="/library" className="text-secondary text-decoration-none">
                                    <i className="bi bi-chevron-right small me-1"></i> My Library
                                </Link>
                            </li>
                        </ul>
                    </div>

                    {/* 3.Social Media */}
                    <div className="col-6 col-md-4">
                        <h6 className="fw-bold text-white mb-3">Connect</h6>
                        <div className="d-flex gap-3 mb-3">
                            <a href="https://github.com/Codyy03" target="_blank" rel="noreferrer" className="text-secondary fs-5" title="GitHub">
                                <i className="bi bi-github"></i>
                            </a>
                            <a href="https://twitter.com" target="_blank" rel="noreferrer" className="text-secondary fs-5" title="Twitter/X">
                                <i className="bi bi-twitter-x"></i>
                            </a>
                            <a href="https://discord.com" target="_blank" rel="noreferrer" className="text-secondary fs-5" title="Discord">
                                <i className="bi bi-discord"></i>
                            </a>
                        </div>
                        <p className="text-secondary small mb-0">
                            <i className="bi bi-envelope me-2"></i>
                            contact@gamerecommender.com
                        </p>
                    </div>
                </div>

                <hr className="border-secondary my-4 opacity-25" />

                <div className="d-flex flex-column flex-md-row justify-content-between align-items-center">
                    <small className="text-secondary mb-2 mb-md-0">
                        &copy; {new Date().getFullYear()} GameRecommender. All rights reserved.
                    </small>
                    <small className="text-secondary text-center text-md-end">
                        Built with <i className="bi bi-heart-fill text-danger mx-1"></i> for gamers.
                    </small>
                </div>
            </div>
        </footer>
    );
}

export default Footer;