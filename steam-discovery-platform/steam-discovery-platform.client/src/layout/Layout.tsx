import { Outlet } from "react-router-dom"; 
import Navbar from "../components/Navbar";
import SearchBar from "../components/SearchBar";

export default function Layout() {
    return (
        <div className="main-bg-gradient min-vh-100">
            <Navbar />
            <SearchBar/>
            <Outlet /> 
        </div>
    );
}