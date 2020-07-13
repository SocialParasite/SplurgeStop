import React from 'react';
import { Header } from './Components/Header';
import { BrowserRouter, Route, Redirect, Switch } from 'react-router-dom';
import { PurchaseTransactionList } from './PurchaseTransaction/PurchaseTransactionList';
import { PurchaseTransactionPage } from './PurchaseTransaction/PurchaseTransactionPage';
import { StoreList } from './StoreProfile/StoreList';
import { StorePage } from './StoreProfile/StorePage';
import { NewStore } from './StoreProfile/NewStore';
import { NewPurchaseTransaction } from './PurchaseTransaction/NewPurchaseTransaction';
import { CityList } from './StoreProfile/CityProfile/CityList';
import { NewCity } from './StoreProfile/CityProfile/NewCity';
import { CityPage } from './StoreProfile/CityProfile/CityPage';
import { CountryList } from './StoreProfile/CountryProfile/CountryList';
import { NewCountry } from './StoreProfile/CountryProfile/NewCountry';
import { CountryPage } from './StoreProfile/CountryProfile/CountryPage';

function App() {
  return (
    <div className="App">
      <Header />
      <BrowserRouter>
        <Switch>
          <Redirect from="/home" to="/" />
          <Route exact path="/" component={PurchaseTransactionList} />
          <Route
            exact
            path="/PurchaseTransaction/Add"
            component={NewPurchaseTransaction}
          />
          <Route
            exact
            path="/PurchaseTransaction/:id"
            component={PurchaseTransactionPage}
          />
          {/* Store */}
          <Route exact path="/Store" component={StoreList} />
          <Route exact path="/Store/Add" component={NewStore} />
          <Route path="/StoreInfo/:id" component={StorePage} />
          {/* City */}
          <Route exact path="/City" component={CityList} />
          <Route exact path="/City/Add" component={NewCity} />
          <Route path="/CityInfo/:id" component={CityPage} />
          {/* Country */}
          <Route exact path="/Country" component={CountryList} />
          <Route exact path="/Country/Add" component={NewCountry} />
          <Route path="/CountryInfo/:id" component={CountryPage} />
        </Switch>
      </BrowserRouter>
    </div>
  );
}

export default App;
