import React from 'react';

interface SearchBarProps {
 //   searchTerm: string;
 //   setSearchTerm: (value: string) => void;
  //  onSearch: () => void;
}

//{ searchTerm, setSearchTerm, onSearch }
const SearchBar: React.FC<SearchBarProps> = () => {
    return (
        < section id="center" className="container mb-4" >
            <div className="row justify-content-center w-100">
                <div className="col-12 col-md-10 col-lg-9">
                    <h1 className="display-4 text-center mb-2 fw-bold text-danger">Steam Discovery</h1>
                    <p className="text-center text-light mb-5 ">Find your next favorite game in our database</p>
                    
                    <div className="input-group input-group-lg shadow-lg search-container">
                        <input
                            type="text"
                            className="form-control bg-secondary text-light border-0 ps-4"
                            placeholder="Search by game name (e.g. Witcher, Portal)..."
                            style={{ boxShadow: 'none' }}
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
        </section >
    );
};

export default SearchBar;