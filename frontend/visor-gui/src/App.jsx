import logo from './logo.svg';
import './App.css';
import React, {Component} from "react";
import { AuthProvider } from "./providers/authProvider";
import { BrowserRouter } from "react-router-dom";
import {Routes} from "./routes/routes";

function App() {
  return (
    <div className="App">
       <AuthProvider>
                    <BrowserRouter children={Routes} basename={"/"} />
            </AuthProvider>
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
