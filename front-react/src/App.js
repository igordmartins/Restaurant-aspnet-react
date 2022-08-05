import './App.css';
import {Home} from './pages/Home';
import {Restaurant} from './pages/Restaurant';
import {BrowserRouter, Route, Routes, NavLink} from "react-router-dom";

function App() {
  return (
    <BrowserRouter>
    <div className="App container">
      <h2 className="d-flex justify-content-center m-3">
        Restaurant React Frontend
      </h2>

      <nav className="navbar navbar-expand-sm bg-light navbar-dark">
        <ul className="navbar-nav">
        <li className="nav-item m-1">
            <NavLink className="btn btn-light btn-outline-primary" to="/home">
              Home
            </NavLink>
          </li>
          <li className="nav-item m-1">
            <NavLink className="btn btn-light btn-outline-primary" to="/restaurant">
              Restaurants
            </NavLink>
          </li>
        </ul>
      </nav>

      <Routes>
        <Route path='/home' element={<Home/>}/>
        <Route path='/restaurant' element={<Restaurant/>}/>
      </Routes>
    </div>
    </BrowserRouter>
  );
}

export default App;
