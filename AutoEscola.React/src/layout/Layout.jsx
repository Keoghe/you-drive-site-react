import { Outlet } from "react-router-dom";
import Header from "../components/Header";
import Footer from "../components/Footer";
import MobileMenu from "../components/MobileMenu";

export default function Layout() {
  return (
    <div className="app-container">
      <Header />

      <main className="main-content">
        <div className="container"> 
          <Outlet />
        </div>
      </main>

      <Footer />
      <MobileMenu />
    </div>
  );
}
