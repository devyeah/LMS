import React, { useState } from 'react';
import { Route, Link } from 'react-router-dom';
import EditPhoto from './EditPhoto';
import editProfile from './EditProfile';
import './account.css';

export default function EditProfileLayout() {
  const [activeMenu, setActiveMenu] = useState('profile');

  return (
    <div id="profileContainer" className="container">
      <div className="row mt-4">
        <div id="menus" className="col-3 list-group">
          <Link 
            id="editProfile"
            className={"list-group-item list-group-item-action " + (activeMenu==="profile" ? "active" : "")}
            onClick={() => setActiveMenu('profile')}
            to="/account/editProfile/profile"
          >
            Edit Profile
          </Link>
          <Link
            id="uploadPhoto"
            className={"list-group-item list-group-item-action " + (activeMenu==="photo" ? "active" : "")}
            onClick={() => setActiveMenu('photo')}
            to="/account/editProfile/photo" 
          >
            Photo
          </Link>
        </div>
        <div id="content" className="col-8">
          <Route path="/account/editProfile/photo" component={EditPhoto} />
          <Route path="/account/editProfile/profile" component={editProfile} />
        </div>
      </div>
    </div>
  );
}