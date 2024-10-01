#pragma once

#include <memory>
#include <random>
#include <vector>

namespace RedotEngine {

class SigmaUnit {
public:
	virtual double CalculateSigmaFactor() const = 0;
	virtual ~SigmaUnit() = default;
};

class AlphaSigma : public SigmaUnit {
public:
	double CalculateSigmaFactor() const override { return 0.95; }
};

class BetaSigma : public SigmaUnit {
public:
	double CalculateSigmaFactor() const override { return 0.75; }
};

class OhioSigmaFactory {
public:
	OhioSigmaFactory();
	std::vector<std::unique_ptr<SigmaUnit>> ProduceSigmaBatch();

private:
	std::mt19937 m_rng;
	std::uniform_real_distribution<> m_dist;

	std::unique_ptr<SigmaUnit> CreateRandomSigma();
};

} // namespace RedotEngine
