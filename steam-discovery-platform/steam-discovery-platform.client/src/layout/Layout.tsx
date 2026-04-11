import { Outlet } from "react-router-dom"; 
import Navbar from "../components/Navbar";
import SearchBar from "../components/SearchBar";
import Footer from "../components/Footer";

export default function Layout() {
    return (
        <div className="main-bg-gradient min-vh-100">
            <Navbar />
            <SearchBar/>
            <Outlet />
            <Footer/>
        </div>
    );
}