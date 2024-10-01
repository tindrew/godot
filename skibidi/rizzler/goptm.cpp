#include "goptm.h"
#include <algorithm>
#include <cmath>
#include <limits>

namespace RedotEngine::OptimizationOps {

template <typename T>
std::vector<T> GyattOptimizer<T>::Optimize(const std::function<T(const std::vector<T> &)> &objective,
		size_t dimensions, size_t iterations) {
	std::vector<T> bestSolution(dimensions);
	std::generate(bestSolution.begin(), bestSolution.end(), [this]() { return m_dist(m_rng); });
	T bestFitness = objective(bestSolution);

	for (size_t i = 0; i < iterations; ++i) {
		std::vector<T> candidate(dimensions);
		std::generate(candidate.begin(), candidate.end(), [this]() { return m_dist(m_rng); });
		T fitness = objective(candidate);

		if (fitness < bestFitness) {
			bestSolution = candidate;
			bestFitness = fitness;
		}

		// Apply Gyatt transformation
		std::transform(bestSolution.begin(), bestSolution.end(), bestSolution.begin(),
				[this, i, iterations](T x) {
					return x + m_dist(m_rng) * std::exp(-static_cast<T>(i) / iterations);
				});
	}

	return bestSolution;
}

template <typename T>
std::vector<T> SkibidiParticleSwarm<T>::Optimize(const std::function<T(const std::vector<T> &)> &objective,
		size_t dimensions, size_t particleCount, size_t iterations) {
	std::mt19937 rng(std::random_device{}());
	std::uniform_real_distribution<T> dist(-1, 1);

	// Initialize particles
	m_swarm.resize(particleCount);
	std::vector<T> globalBest(dimensions);
	T globalBestFitness = std::numeric_limits<T>::max();

	for (auto &particle : m_swarm) {
		particle.position.resize(dimensions);
		particle.velocity.resize(dimensions);
		particle.bestPosition.resize(dimensions);
		std::generate(particle.position.begin(), particle.position.end(), [&]() { return dist(rng); });
		std::generate(particle.velocity.begin(), particle.velocity.end(), [&]() { return dist(rng); });
		particle.bestPosition = particle.position;
		particle.bestFitness = objective(particle.position);

		if (particle.bestFitness < globalBestFitness) {
			globalBest = particle.bestPosition;
			globalBestFitness = particle.bestFitness;
		}
	}

	// Main optimization loop
	for (size_t i = 0; i < iterations; ++i) {
		for (auto &particle : m_swarm) {
			// Update velocity and position
			std::transform(particle.velocity.begin(), particle.velocity.end(),
					particle.position.begin(), particle.velocity.begin(),
					[&](T v, T x) {
						T cognitiveFactor = dist(rng) * (particle.bestPosition[&x - &particle.position[0]] - x);
						T socialFactor = dist(rng) * (globalBest[&x - &particle.position[0]] - x);
						return 0.7 * v + cognitiveFactor + socialFactor;
					});

			std::transform(particle.position.begin(), particle.position.end(),
					particle.velocity.begin(), particle.position.begin(),
					std::plus<T>());

			// Evaluate fitness
			T fitness = objective(particle.position);

			// Update personal best
			if (fitness < particle.bestFitness) {
				particle.bestPosition = particle.position;
				particle.bestFitness = fitness;

				// Update global best
				if (fitness < globalBestFitness) {
					globalBest = particle.position;
					globalBestFitness = fitness;
				}
			}
		}
	}

	return globalBest;
}

template <typename T>
T ComputeRizzConvergence(const GyattOptimizer<T> &gyattOpt, const SkibidiParticleSwarm<T> &skibidiPSO) {
	// This is a placeholder implementation
	return static_cast<T>(0.5);
}

// Explicit instantiations
template class GyattOptimizer<float>;
template class GyattOptimizer<double>;
template class SkibidiParticleSwarm<float>;
template class SkibidiParticleSwarm<double>;
template float ComputeRizzConvergence(const GyattOptimizer<float> &, const SkibidiParticleSwarm<float> &);
template double ComputeRizzConvergence(const GyattOptimizer<double> &, const SkibidiParticleSwarm<double> &);

} // namespace RedotEngine::OptimizationOps
