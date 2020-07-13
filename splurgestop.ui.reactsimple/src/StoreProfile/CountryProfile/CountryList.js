import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from './../../Components/Page';
import { deleteCountry } from './CountryCommands';

export function CountryList() {
  const [countries, setCountries] = useState(null);
  const [countriesLoading, setCountriesLoading] = useState(true);

  useEffect(() => {
    const loadCountries = async () => {
      const url = 'https://localhost:44304/api/Country';
      const response = await fetch(url);
      const data = await response.json();
      setCountries(data);
      setCountriesLoading(false);
    };

    loadCountries();
  }, []);

  const removeItem = (index) => {
    let data = countries.filter((_, i) => i !== index);
    setCountries(data);
  };

  const handleDelete = async (country) => {
    let index = countries.findIndex((s) => s.id === country.id);
    removeItem(index);

    await deleteCountry({
      id: country.id,
    });
  };

  return (
    <Page title="Countries">
      <Link
        css={css`
          text-decoration: none;
        `}
        to={`Country/Add`}
      >
        {' '}
        <Button className="float-right">Add Country</Button>
      </Link>
      <div
        css={css`
          margin: 50px auto 20px auto;
          padding: 30px 12px;
        `}
      >
        <div
          css={css`
            display: flex;
            align-items: center;
            justify-content: space-between;
          `}
        >
          <title>Countries</title>
        </div>
        {countriesLoading ? (
          <div
            css={css`
              font-size: 16px;
              font-style: italic;
            `}
          >
            Loading...
          </div>
        ) : (
          <Table bordered hover size="sm">
            <thead>
              <tr
                css={css`
                  background: burlywood;
                  text-align: center;
                  text-transform: uppercase;
                `}
              >
                <th>Country name</th>
                <th>Details</th>
                <th>Remove</th>
              </tr>
            </thead>
            {countries.map((country) => (
              <tbody key={country.id}>
                <tr>
                  <Fragment key={country.id}>
                    <td>
                      <Link
                        css={css`
                          text-decoration: none;
                        `}
                        to={`CountryInfo/${country.id}`}
                      >
                        {country.name}
                      </Link>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button variant="info" href={`CountryInfo/${country.id}`}>
                        Show
                      </Button>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button
                        variant="danger"
                        onClick={() => {
                          handleDelete(country);
                        }}
                      >
                        Delete
                      </Button>
                    </td>
                  </Fragment>
                </tr>
              </tbody>
            ))}
          </Table>
        )}
      </div>
    </Page>
  );
}
