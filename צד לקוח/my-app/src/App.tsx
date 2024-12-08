import React from 'react';
import './App.css';
import EmailBox from './components/EmailBox';
import SignIn from './components/SignIn';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import CSSGrid from './components/Grid';
import Autocomplete from '@mui/material/Autocomplete';

function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path='/home' element={<CSSGrid></CSSGrid> } />
          <Route path='/' element={<SignIn></SignIn>} />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
