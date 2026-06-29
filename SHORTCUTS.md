# Shortcuts & Decisions

This document describes known shortcuts taken in this solution and the reasoning behind them.

## JWT Bearer instead of API key

The task specifies API key protection. I chose JWT Bearer authentication instead, as it is a more standard .NET pattern and provides built-in expiry and revocation without requiring config changes. The trade-off is one extra `/token` endpoint for obtaining a token.

## `/token` endpoint in the API

The `/token` endpoint is a development convenience to make the API testable without an external identity provider. In production, tokens would be issued by a dedicated identity provider (e.g. Azure AD / Entra ID), and this endpoint would be removed.

## JWT key and password hardcoded in `appsettings.json`

The JWT signing key and demo password are stored in `appsettings.json` for simplicity. In production these would be stored in Azure Key Vault or environment-specific secrets, never in source control.

## CORS allows any origin

CORS is configured with `AllowAnyOrigin` to simplify local development. In production this would be restricted to known frontend domains.

## No structured logging

`ILogger` is not wired up. Errors surface in the console output only. In production, structured logging (e.g. Serilog to Application Insights) would be added.

## No caching

SMHI weather data is fetched on every request. Since the data does not change every second, `IMemoryCache` with a short TTL (e.g. 60 seconds) would meaningfully reduce load on the SMHI API in a production scenario.

## No resilience / retry policy

If the SMHI API is temporarily unavailable, the request fails immediately. A production implementation would use `AddStandardResilienceHandler()` (Polly) to add retries with exponential backoff.

## Minimal test coverage

Two unit tests cover the core merge logic in `MeasurementService`. A production codebase would include integration tests against the real SMHI API and additional edge cases (e.g. both datasets empty, SMHI returning unexpected values).
