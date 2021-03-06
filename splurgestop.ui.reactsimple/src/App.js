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
import { ProductList } from './ProductProfile/ProductList';
import { NewProduct } from './ProductProfile/NewProduct';
import { ProductPage } from './ProductProfile/ProductPage';
import { BrandList } from './ProductProfile/BrandProfile/BrandList';
import { NewBrand } from './ProductProfile/BrandProfile/NewBrand';
import { BrandPage } from './ProductProfile/BrandProfile/BrandPage';
import { ProductTypeList } from './ProductProfile/ProductTypeProfile/ProductTypeList';
import { NewProductType } from './ProductProfile/ProductTypeProfile/NewProductType';
import { ProductTypePage } from './ProductProfile/ProductTypeProfile/ProductTypePage';
import { LocationList } from './StoreProfile/LocationProfile/LocationList';
import { NewLocation } from './StoreProfile/LocationProfile/NewLocation';
import { LocationPage } from './StoreProfile/LocationProfile/LocationPage';

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
          {/* Location */}
          <Route exact path="/Location" component={LocationList} />
          <Route exact path="/Location/Add" component={NewLocation} />
          <Route path="/LocationInfo/:id" component={LocationPage} />
          {/* City */}
          <Route exact path="/City" component={CityList} />
          <Route exact path="/City/Add" component={NewCity} />
          <Route path="/CityInfo/:id" component={CityPage} />
          {/* Country */}
          <Route exact path="/Country" component={CountryList} />
          <Route exact path="/Country/Add" component={NewCountry} />
          <Route path="/CountryInfo/:id" component={CountryPage} />
          {/* Product */}
          <Route exact path="/Product" component={ProductList} />
          <Route exact path="/Product/Add" component={NewProduct} />
          <Route path="/ProductInfo/:id" component={ProductPage} />
          {/* Brand */}
          <Route exact path="/Brand" component={BrandList} />
          <Route exact path="/Brand/Add" component={NewBrand} />
          <Route path="/BrandInfo/:id" component={BrandPage} />
          {/* Product type */}
          <Route exact path="/ProductType" component={ProductTypeList} />
          <Route exact path="/ProductType/Add" component={NewProductType} />
          <Route path="/ProductTypeInfo/:id" component={ProductTypePage} />
        </Switch>
      </BrowserRouter>
    </div>
  );
}

export default App;
