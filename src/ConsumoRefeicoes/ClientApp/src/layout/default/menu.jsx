import {React, useContext} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import MenuItem from './menu-item';
import TokenAPI from '../../lib/api/token';
import { AuthContext } from '../../lib/context/auth-context';

import './menu.css';

const Menu = () => {
  const { dadosUser } = useContext(AuthContext);
  const storage = TokenAPI.getToken();
  const RemoveStorage = () => {
    TokenAPI.removeToken();
  };
  return (
<>
      <p className="color">
        {dadosUser?.name || storage?.name}</p>    
    <ul className="navbar-nav">  
      <MenuItem label="Sair" link="/login" onClick={RemoveStorage} />
    </ul>
    </>
  
  );
};

export default Menu;
